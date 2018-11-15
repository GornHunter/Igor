using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string AdminBan(string ou, string publisher);

        [OperationContract]
        string[] CheckPublisherSpam(string ou);

        [OperationContract]
        string PublisherAddTheme(string ou, string cn, string themeName);

        [OperationContract]
        string PublisherModifyTheme(string ou, string cn, string oldThemeName, string newThemeName);

        [OperationContract]
        string PublisherRemoveTheme(string ou, string cn, string themeName);

        [OperationContract]
        string PublisherReadTheme(string ou, string cn);

        [OperationContract]
        string PublisherWriteTheme(string ou, string cn);

        [OperationContract]
        string PublisherSendNotifications(string ou, string cn, string themeName, string title, string content, string timeStamp);

        [OperationContract]
        string SubscriberReadTheme(string ou);

        [OperationContract]
        string Subscribe(string ou, string cn, string themeName);

        [OperationContract]
        string Unsubscribe(string ou, string cn, string themeName);

        [OperationContract]
        string SubscriberReadNotifications(string ou, string cn);

        [OperationContract]
        string SubscriberCheckUpdates(string ou, string cn);

        [OperationContract]
        bool CheckIfPublisherBanned(string cn);
    }
}