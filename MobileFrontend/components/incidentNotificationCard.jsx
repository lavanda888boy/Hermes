import { Card } from "react-native-paper";
import { Text } from "react-native";

const IncidentNotificationCard = ({ incidentNotification }) => {
  const getNotificationSubtitle = () => {
    let subtitle = "";

    switch (incidentNotification.Severity) {
      case "HIGH":
        subtitle = "🔴 High severity incident";
        break;
      case "MEDIUM":
        subtitle = "🟡 Medium severity incident";
        break;
      case "LOW":
        subtitle = "🟢 Low severity incident";
        break;
      default:
        return "Unknown severity incident";
    }

    return subtitle + ` within ${incidentNotification.AreaRadius} km radius`;
  }

  const formatNotificationTimestamp = () => {
    return new Date(incidentNotification.Timestamp).toLocaleString("en-GB", {
      year: "numeric",
      month: "short",
      day: "2-digit",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false
    })
  }

  return (
    <Card>
      <Card.Title
        title={incidentNotification.Category}
        subtitle={getNotificationSubtitle()}
      />

      <Card.Content>
        {incidentNotification.Note !== "" && (
          <Text className="text-base text-justify text-gray-400 uppercase font-ops-it mb-2">
            Note: {incidentNotification.Note}
          </Text>
        )}

        <Text className="text-base text-justify font-ops-r">
          {incidentNotification.Description}
        </Text>
      </Card.Content>

      <Card.Actions>
        <Text className="text-base text-end text-gray-600 font-ops-it">
          {formatNotificationTimestamp()}
        </Text>
      </Card.Actions>
    </Card>
  );
}

export default IncidentNotificationCard;