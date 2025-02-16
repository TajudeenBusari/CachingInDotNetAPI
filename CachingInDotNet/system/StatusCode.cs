//<copyright file="StatusCode" Owner=tjtechy> 
//Author: Tajudeen Busari
//Date: 2025-14-01
//</copyright>
namespace CachingInDotNet.system;
public class StatusCode
{
    public static readonly int SUCCESS = 200;
    public static readonly int CREATED = 201;
    public static readonly int NO_CONTENT = 204;
    public static readonly int BAD_REQUEST = 400;
    public static readonly int NOT_FOUND = 404;
    public static readonly int INTERNAL_SERVER_ERROR = 500;
    public static readonly int NOT_IMPLEMENTED = 501;
    public static readonly int SERVICE_UNAVAILABLE = 503;
    public static readonly int GATEWAY_TIMEOUT = 504;
    public static readonly int UNPROCESSABLE_ENTITY = 422;
    public static readonly int CONFLICT = 409;
    public static readonly int UNAUTHORIZED = 401;
    public static readonly int FORBIDDEN = 403;
    public static readonly int PRECONDITION_FAILED = 412;
    public static readonly int UNSUPPORTED_MEDIA_TYPE = 415;
    public static readonly int TOO_MANY_REQUESTS = 429;
    public static readonly int REQUEST_TIMEOUT = 408;
    public static readonly int PAYLOAD_TOO_LARGE = 413;
    public static readonly int NOT_ACCEPTABLE = 406;
    public static readonly int METHOD_NOT_ALLOWED = 405;
    public static readonly int LENGTH_REQUIRED = 411;
    public static readonly int GONE = 410;
    public static readonly int EXPECTATION_FAILED = 417;
    public static readonly int REQUEST_ENTITY_TOO_LARGE = 413;
    public static readonly int REQUEST_URI_TOO_LONG = 414;
    public static readonly int REQUESTED_RANGE_NOT_SATISFIABLE = 416;
    public static readonly int HTTP_VERSION_NOT_SUPPORTED = 505;
    public static readonly int NETWORK_AUTHENTICATION_REQUIRED = 511;
    public static readonly int PERMANENT_REDIRECT = 308;
    public static readonly int TEMPORARY_REDIRECT = 307;
    public static readonly int MOVED_PERMANENTLY = 301;
    public static readonly int FOUND = 302;
    public static readonly int SEE_OTHER = 303;
    public static readonly int NOT_MODIFIED = 304;
    public static readonly int USE_PROXY = 305;
    public static readonly int SWITCH_PROXY = 306;
    public static readonly int MULTIPLE_CHOICES = 300;
    public static readonly int CONTINUE = 100;
    public static readonly int SWITCHING_PROTOCOLS = 101;
    public static readonly int PROCESSING = 102;
    public static readonly int EARLY_HINTS = 103;
    public static readonly int MULTI_STATUS = 207;
    
}