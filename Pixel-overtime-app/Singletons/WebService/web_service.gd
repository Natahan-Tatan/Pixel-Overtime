extends Node

@export var instance_host: String:
	get: 
		return _instance_domain
	set(value):
		var splitted = value.split(":")
		_instance_domain = "%s:%s" % [splitted[0], splitted[1]]

		if(splitted.size() > 2):
			_instance_port = int(splitted[2])


var _instance_domain:= ""
var _instance_port:= 443

@export_group("API routes")

@export_subgroup("User")
@export var route_register:= ApiRoute.new()

@export var route_login:= ApiRoute.new()
@export var route_refresh:= ApiRoute.new()
@export var route_logout:= ApiRoute.new()

@export var route_get_user_infos:= ApiRoute.new()
@export var route_set_user_infos:= ApiRoute.new()

@export_subgroup("Time")
@export var route_get_times_list:= ApiRoute.new()
@export var route_get_time:= ApiRoute.new()
@export var route_create_time:= ApiRoute.new()
@export var route_delete_time:= ApiRoute.new()
@export var route_update_time:= ApiRoute.new()

const USER_SAVE_FILE:= "user://user.res";

var user: User

signal api_error(title: String, message: String)

func _ready() -> void:
	_load_user()

	self.login("nathan@test.fr", "Azerty1234&") #FIXME: Remove this

func _load_user():
	if FileAccess.file_exists(USER_SAVE_FILE):
		user = load(USER_SAVE_FILE)
	else:
		user = User.new()

func _save_user():
	ResourceSaver.save(user, USER_SAVE_FILE)

func _do_request(caller: StringName, route: ApiRoute, query_string: Dictionary, json_data: String, use_auth: bool, reauth_on_401: bool = true) -> PackedByteArray:
	var client = HTTPClient.new()

	var error:= client.connect_to_host(_instance_domain, _instance_port);
	if (error != OK):
		emit_signal("api_error", "Web service: "+caller, "Error when connect to host "+instance_host+": "+error_string(error))

	while client.get_status() == HTTPClient.STATUS_CONNECTING or client.get_status() == HTTPClient.STATUS_RESOLVING:
		client.poll()
		await get_tree().process_frame
	
	var headers = [
		"User-Agent: OverPixel-Godot_Client_v" + ProjectSettings.get_setting("application/config/version") + OS.get_name() + "-" + OS.get_model_name()
	]

	if(route.method == HTTPClient.METHOD_POST or route.method == HTTPClient.METHOD_PATCH or  route.method == HTTPClient.METHOD_PUT):
		headers.append("Content-Type: application/json")

	if(use_auth):
		headers.append("Authorization: Bearer %s" % user.bearer)

	var request_uri = route.route

	if(query_string.size() > 0):
		request_uri += "?"
		for key in query_string:
			request_uri += ("%s=%s&" % [key, str(query_string[key])]).uri_encode()

	print(request_uri)
	
	error = client.request(int(route.method), request_uri, headers, json_data)
	if (error != OK):
		emit_signal("api_error", "Web service: " + caller, "Error when create request to " + request_uri.split("?")[0] + ": " + error_string(error))

	while client.get_status() == HTTPClient.STATUS_REQUESTING:
		client.poll()
		await get_tree().process_frame

	var response = PackedByteArray()

	if(client.has_response()):
		
		while client.get_status() == HTTPClient.STATUS_BODY:
			client.poll()
			var chunk = client.read_response_body_chunk()
			if chunk.size() == 0:
				await get_tree().process_frame
			else:
				response = response + chunk

		# Retry if received unauthorise response
		if(use_auth and reauth_on_401 and client.get_response_code() == 401):
			var refreshResponse = await self._do_request(caller, route_refresh, {}, JSON.stringify({"refreshToken": user.refresh}), false, false)

			if(refreshResponse.size() > 0):
				var dict = JSON.parse_string(refreshResponse.get_string_from_utf8())
				user.bearer = dict["accessToken"]
				user.refresh = dict["refreshToken"]
				self._save_user()
			
			response = await self._do_request(caller, route, query_string, json_data, false, false)

		if(client.get_response_code() >= 400):
			emit_signal("api_error", "Web service: "+caller, "Request returns error " + request_uri.split("?")[0] + " (" + str(client.get_response_code()) + "): " + response.get_string_from_utf8())
	
	client.close();
	return response;


func register(email: String, password: String, username: String):
	pass

func login(email: String, password: String):
	var body = JSON.stringify({
			"email": email,
			"password": password,
		})

	var response = await self._do_request("Login", route_login, {}, body, false, false)

	var dict = JSON.parse_string(response.get_string_from_utf8())
	user.bearer = dict["accessToken"]
	user.refresh = dict["refreshToken"]

	# Update user infos
	await get_me_infos()

	print_rich("""[u][b]Current logged user[/b][/u]
	[b]Id:[/b] %s
	[b]Email:[/b] %s
	[b]Email:[/b] %s
	[b]Account created:[/b] %s
	""" % [
		user.id,
		user.email,
		str(user.email_confirmed),
		Time.get_datetime_string_from_datetime_dict(user.account_created_date, true)
	])

func get_me_infos():
	var response = await self._do_request("GetMeInfos", route_get_user_infos, {}, "", true, true)
	var dict = JSON.parse_string(response.get_string_from_utf8())
	
	user.id = dict["id"]
	user.name = dict["name"]
	user.email = dict["email"]
	user.email_confirmed = dict["emailConfirmed"]
	user.account_created_date = Time.get_datetime_dict_from_datetime_string(dict["accountCreatedAt"], false)

	user.last_check_time = Time.get_unix_time_from_system()

	self._save_user()

	return user

func logout():
	pass

#TODO: add filters and pagination
func get_times_list():
	pass

func _on_api_error(title:String, message:String) -> void:
	printerr("[API Error] %s\n\t%s" % [title, message])
