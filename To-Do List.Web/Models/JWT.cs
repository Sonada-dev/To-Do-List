namespace To_Do_List.Web.Models
{
    public class JWT
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JWT(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Token
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"] ?? string.Empty;
            }
            set { }
        }
    }
}
