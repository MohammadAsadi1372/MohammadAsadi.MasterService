using MasterService.EndPoint.Api.Models;

namespace MasterService.EndPoint.Api.Helper
{
    public class H_JWT
    {
        private string _username;
        private string _password;
        private DateTime _expireTime;
        private readonly string _systemname;
        private readonly IConfiguration _config;

        public H_JWT(IConfiguration config, string systemName)
        {
            _config = config;
            _systemname = systemName;
        }

        public H_JWT(string Username, string Password, DateTime ExpireTime, string Systemname, IConfiguration config)
        {
            _username = Username;
            _password = Password;
            _expireTime = ExpireTime;
            _systemname = Systemname;
            _config = config;
        }

        public Tuple<string, bool> Generator()
        {
            if (string.IsNullOrEmpty(_username))
                return new Tuple<string, bool>("مقدار نام کاربری درج نگردیده است", false);

            var Data = _config.GetSection("Services").Get<List<M_Services>>();
            foreach (var item in Data)
                if (item.ServiceName == _systemname)
                    foreach (var li in item.Login)
                    {
                        if (li.Username == _username && li.Password == _password)
                        {
                            string Expire = _expireTime.Ticks.ToString();
                            string TempToken = _username + "|" + Expire + "|" + _password;
                            return new Tuple<string, bool>(new EncryptionHelper().Encrypt(TempToken, "Ok@jWt$@saDiWT#2"), true);
                        }
                    }
            return new Tuple<string, bool>("نام کاربری در سیستم موجود نیست", false);
        }

        public bool ISValidToken(string Token)
        {
            try
            {
                string Result = new EncryptionHelper().Decrypt(Token, "Ok@jWt$@saDiWT#2");
                string[] Splited = Result.Split('|');
                string Username = Splited[0];
                decimal ExpireTime = Convert.ToDecimal(Splited[1]);
                string Password = Splited[2];

                if (ExpireTime < DateTime.Now.Ticks)
                    return false;
                else
                {
                    var Data = _config.GetSection("Services").Get<List<M_Services>>();
                    foreach (var item in Data)
                        if (item.ServiceName == _systemname)
                            foreach (var li in item.Login)
                                if (li.Username == Username && li.Password == Password)
                                    return true;
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
