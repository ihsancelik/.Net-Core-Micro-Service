using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Helpers.Message
{
    public abstract class MessageGenerator
    {
        public static string Generate(string objectName, MessageGeneratorActions action)
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

                case MessageGeneratorActions.Wrong:
                    return objectName + " wrong!";

                default:
                    return "";
            }
        }
    }

    public enum MessageGeneratorActions
    {
        Created,
        Deleted,
        Updated,
        Exist,
        NotFound,
        Empty,
        Expired,
        AlreadyUsed,
        Invalid,
        IsNotActive,
        Wrong
    }
}
