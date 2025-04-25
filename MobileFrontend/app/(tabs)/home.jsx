import { View, Text, TouchableOpacity, Modal } from "react-native";
import { useState, useEffect } from "react";
import { notificationListener } from "../../config/notificationEmitter";
import AsyncStorage from "@react-native-async-storage/async-storage";
import IncidentReportForm from "../../components/incidentReportForm";
import IncidentNotificationCard from "../../components/incidentNotificationCard";

const Home = () => {
  const [incidentReportFormVisible, setIncidentReportFormVisible] = useState(false);
  const [notifications, setNotifications] = useState([]);

  useEffect(() => {
    const handleNotificationUpdate = (updatedNotifications) => {
      setNotifications(updatedNotifications);
    };

    notificationListener.on("notificationsUpdated", handleNotificationUpdate);

    const fetchNotifications = async () => {
      const savedNotifications = await AsyncStorage.getItem("notifications");
      const parsedNotifications = savedNotifications ? JSON.parse(savedNotifications) : [];

      setNotifications(parsedNotifications);
    };

    fetchNotifications();

    return () => {
      notificationListener.off("notificationsUpdated", handleNotificationUpdate);
    };
  }, []);


  const openIncidentReportForm = () => {
    setIncidentReportFormVisible(true);
  }

  return (
    <View className="flex-1 justify-between bg-white">
      <View className="flex-1 p-4">
        {notifications.length > 0 ? (
          <View className="flex-1 items-start">
            {notifications.map((notification, index) => (
              <IncidentNotificationCard key={index} incidentNotification={notification} />
            ))}
          </View>
        ) : (
          <View className="flex-1 justify-center items-center">
            <Text className="text-center text-lg font-ops-r text-gray-400">
              Here will appear information about the incidents that may have affected you.
            </Text>
          </View>
        )}
      </View>

      <TouchableOpacity
        className="rounded-3xl w-[180px] py-3 ml-4 mb-10 bg-primary"
        activeOpacity={0.7}
        onPress={openIncidentReportForm}
      >
        <Text className="text-xl text-center text-white font-ops-sb">
          Report incident
        </Text>
      </TouchableOpacity>

      <Modal
        visible={incidentReportFormVisible}
        animationType="fade"
        transparent={true}
        onRequestClose={() => {
          setIncidentReportFormVisible(false);
        }}
      >
        <View className="flex-1 justify-center items-center bg-black/30">
          <IncidentReportForm onClose={() => setIncidentReportFormVisible(false)} />
        </View>
      </Modal>
    </View >
  );
};

export default Home;