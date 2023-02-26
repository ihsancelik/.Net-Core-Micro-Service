using Miracle.Api.Enums;

namespace Miracle.Api.Services.Helpers
{
    public interface IMessageGeneratorService
    {
        /// <summary>
        /// objectName parametresine özel action parametresi yardımıyla bir mesaj oluşturur.
        /// Örn: (User, Notfound) parametreleri verildiğinde "User not found" değeri döner.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string PrepareResponseMessage(string objectName, MessageGeneratorActions action);
    }
}
