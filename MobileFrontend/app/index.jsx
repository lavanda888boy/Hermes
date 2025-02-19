import { View, Text } from "react-native";
import React from "react";
import { SafeAreaView } from "react-native-safe-area-context";

const InitScreen = () => {
  return (
    <SafeAreaView>
      <View>
        <Text className="font-ops-it">Init Screen</Text>
      </View>
    </SafeAreaView>
  );
};

export default InitScreen;
