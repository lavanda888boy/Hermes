import { Tabs } from "expo-router";
import { Pressable, SafeAreaView } from "react-native";
import FontAwesome from '@expo/vector-icons/FontAwesome';
import MaterialCommunityIcons from '@expo/vector-icons/MaterialCommunityIcons';
import { useColorScheme } from "nativewind";

const TabLayout = () => {
  const colorScheme = useColorScheme();

  return (
    <SafeAreaView className="flex-1">
      <Tabs
        screenOptions={{
          tabBarActiveTintColor: colorScheme.primary,
        }}
      >
        <Tabs.Screen
          name="home"
          options={{
            title: "Home",
            headerShown: false,
            tabBarIcon: ({ color }) => <FontAwesome size={28} name="home" color={color} />,
            tabBarButton: (props) => (
              <Pressable {...props} />
            ),
          }}
        />
        <Tabs.Screen
          name="profile"
          options={{
            title: "Profile",
            headerShown: false,
            tabBarIcon: ({ color }) => <MaterialCommunityIcons size={28} name="account" color={color} />,
            tabBarButton: (props) => (
              <Pressable {...props} />
            ),
          }}
        />
      </Tabs >
    </SafeAreaView>
  );
};

export default TabLayout;