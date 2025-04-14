extends Resource

class_name ApiRoute

enum Method {
    GET = HTTPClient.METHOD_GET, 
    POST = HTTPClient.METHOD_POST, 
    PUT = HTTPClient.METHOD_PUT, 
    PATCH = HTTPClient.METHOD_PATCH,
    DELETE = HTTPClient.METHOD_DELETE
}

@export var method:= Method.GET
@export var route:= ""