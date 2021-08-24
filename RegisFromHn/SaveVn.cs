using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RegisFromHn
{
    class SaveVn
    {
        static readonly HttpClient client = new HttpClient();

        public async Task<string> save(string posturi, string idcard, int appointRowId, string userPtRight = null, string exType = "ex04")
        {
            string content = null;
            try
            {
                saveVn savevn = new saveVn();
                savevn.Idcard = idcard;
                savevn.appointId = appointRowId;
                savevn.exType = exType;
                savevn.userPtRight = userPtRight;

                var response = await client.PostAsJsonAsync(posturi, savevn);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Message :{0} ", ex.Message);
            }
            return content;
        }
    }
}
