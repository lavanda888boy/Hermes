import { View, Text, TouchableOpacity, Modal } from "react-native";
import { useState } from "react";
import IncidentReportForm from "../../components/incidentReportForm";

const Home = () => {
  const [incidentReportFormVisible, setIncidentReportFormVisible] = useState(false);

  const openIncidentReportForm = () => {
    setIncidentReportFormVisible(true);
  }

  return (
    <View className="flex-1 justify-between bg-white">
      <View></View>

      <TouchableOpacity
        className="rounded-3xl w-[180px] py-3 ml-4 mb-10 bg-primary"
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
          <IncidentReportForm />
        </View>
      </Modal>
    </View>
  );
};

export default Home;