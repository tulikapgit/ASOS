# Managed Identity
Managed identities for Azure resources is a cross-Azure feature that enables you to create a secure identity associated with the deployment under which your application code runs. You can then associate that identity with access-control roles that grant custom permissions for accessing specific Azure resources that your application needs.
With managed identities, the Azure platform manages this runtime identity. You do not need to store and protect access keys in your application code or configuration, either for the identity itself, or for the resources you need to access. A Service Bus client app running inside an Azure App Service application with enabled managed entities for Azure resources support does not need to handle SAS rules and keys, or any other access tokens. The client app only needs the endpoint address of the Service Bus Messaging namespace. Once it is associated with a managed identity, your Service Bus client can do all authorized operations. Authorization is granted by associating a managed entity with Service Bus roles.
Azure provides the below Azure built-in roles for authorizing access to a Service Bus namespace:
- Azure Service Bus Data Owner: Use this role to allow full access to Service Bus namespace and its entities (queues, topics, subscriptions, and filters)
- Azure Service Bus Data Sender: Use this role to allow sending messages to Service Bus queues and topics.
- Azure Service Bus Data Receiver: Use this role to allow receiving messages from Service Bus queues and subscriptions.

To use Service Bus with managed identities, you need to assign the appservice where the Azure Function is hosted the role and the appropriate scope. Create an Http triggered function and once its hosted in an App Service follow these steps:

1. Go to Settings and select Identity.

2. Select the Status to be On.

3. Select Save to save the setting.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://learn.microsoft.com/en-us/azure/service-bus-messaging/media/service-bus-managed-service-identity/identity-web-app.png">
  <source media="(prefers-color-scheme: light)" srcset="https://learn.microsoft.com/en-us/azure/service-bus-messaging/media/service-bus-managed-service-identity/identity-web-app.png">
  <img alt="Shows an illustrated sun in light mode and a moon with stars in dark mode." src="https://learn.microsoft.com/en-us/azure/service-bus-messaging/media/service-bus-managed-service-identity/identity-web-app.png">
</picture>

Once you've enabled this setting, a new service identity is created in your Azure Active Directory (Azure AD) and configured into the App Service host. Assign one of the Service Bus roles to the managed service identity to run the differnt Azure Functions.
On the overview page, select Access control (IAM) from the left-hand menu.
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/rg-access-control.png">
  <source media="(prefers-color-scheme: light)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/rg-access-control.png">
  <img alt="Shows an illustrated sun in light mode and a moon with stars in dark mode." src="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/rg-access-control.png">
</picture>
On the Access control (IAM) page, select the Role assignments tab. Select Add from the top menu and then Add role assignment from the resulting drop-down menu.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/add-role-assignment-menu.png">
  <source media="(prefers-color-scheme: light)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/add-role-assignment-menu.png">
  <img alt="Shows an illustrated sun in light mode and a moon with stars in dark mode." src="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/add-role-assignment-menu.png">
</picture>

Use the search box to filter the results to the desired role. For this example, search for Azure Service Bus Data Owner and select the matching result. Then choose Next.
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/roles.png">
  <source media="(prefers-color-scheme: light)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/roles.png">
  <img alt="Shows an illustrated sun in light mode and a moon with stars in dark mode." src="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/roles.png">
</picture>
Under Assign access to, select User, group, or service principal, and then choose + Select members.
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/members.png">
  <source media="(prefers-color-scheme: light)" srcset="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/members.png">
  <img alt="Shows an illustrated sun in light mode and a moon with stars in dark mode." src="https://learn.microsoft.com/en-us/azure/role-based-access-control/media/shared/members.png">
</picture>
Select Review + assign to go to the final page, and then Review + assign again to complete the process.
