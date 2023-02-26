using Miracle.Api.Enums;

namespace Miracle.Api.Services.Helpers
{
    public class MessageGeneratorService : IMessageGeneratorService
    {
        /// <summary>
        /// objectName parametresine özel action parametresi yardımıyla bir mesaj oluşturur.
        /// Örn: (User, Notfound) parametreleri verildiğinde "User not found" değeri döner.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string PrepareResponseMessage(string objectName, MessageGeneratorActions action)
        {
            switch (action)
            {
                case MessageGeneratorActions.Created:
                    return objectName + " created.";

                case MessageGeneratorActions.Deleted:
                    return objectName + " deleted.";

                case MessageGeneratorActions.Updated:
                    return objectName + " updated.";

                case MessageGeneratorActions.NotFound:
                    return objectName + " not found!";

                case MessageGeneratorActions.Empty:
                    return objectName + " cannot be empty!";

                case MessageGeneratorActions.Exist:
                    return objectName + " already exist!";

                case MessageGeneratorActions.Expired:
                    return objectName + " expired!";

                case MessageGeneratorActions.AlreadyUsed:
                    return objectName + " already used!";

                case MessageGeneratorActions.Invalid:
                    return objectName + " invalid";

                case MessageGeneratorActions.IsNotActive:
                    return objectName + " not active!";

                default:
                    return "";
            }
        }
    }
}
