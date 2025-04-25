import { Controller, useForm } from "react-hook-form";
import { View, Text, TextInput, Keyboard } from "react-native";
import { Picker } from "@react-native-picker/picker";
import SubmitButton from "./submitButton";
import { incidentRegistrationApi } from "../config/axios";
import { useState, useContext } from "react";
import { ApiContext } from "../config/apiContext";
import SnackBarMessage from "./snackBarMessage";

const IncidentReportForm = ({ onClose }) => {
  const { control, handleSubmit, formState: { errors, isValid } } = useForm({
    mode: "all",
    reValidateMode: "onChange"
  });

  const { fcmToken, allIncidentCategories } = useContext(ApiContext);

  const [snackbarVisible, setSnackbarVisible] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("");

  const onIncidentReportFormSubmit = async (data) => {
    try {
      data["userToReport"] = fcmToken;
      await incidentRegistrationApi.post("/", data);

      setSnackbarMessage("✅ Thank you for your report!");
    } catch (error) {
      console.error(error);

      const statusCode = error.response?.status;

      if (statusCode === 400) {
        setSnackbarMessage("📝 This incident was already reported.");
      } else {
        setSnackbarMessage("⛔ An error occurred. Try again later.");
      }
    } finally {
      Keyboard.dismiss();

      setSnackbarVisible(true);

      setTimeout(() => {
        setSnackbarVisible(false);
        onClose();
      }, 3000);
    }
  }

  return (
    <>
      <View className="flex w-[90%] justify-center p-6 rounded-2xl elevation-md bg-white">
        <Text className="text-4xl text-center text-primary font-ops-sb">
          New incident
        </Text>

        <Text className="mt-4 mb-1 font-ops-r text-lg text-gray-700">
          Category
        </Text>

        <Controller
          control={control}
          name="category"
          defaultValue="Something"
          render={({ field: { onChange, value } }) => (
            <View className="border border-gray-300 rounded-md">
              <Picker
                mode="dropdown"
                selectedValue={value}
                onValueChange={(itemValue) => onChange(itemValue)}
              >
                {allIncidentCategories.map((category, index) => (
                  <Picker.Item
                    key={index}
                    label={category}
                    value={category}
                  />
                ))}

                <Picker.Item label="Other" value="Other" />
              </Picker>
            </View>
          )}
        />

        <Text className="mt-4 mb-1 font-ops-r text-lg text-gray-700">
          Description
        </Text>

        <Controller
          control={control}
          name="description"
          render={({ field: { onChange, onBlur, value } }) => (
            <TextInput
              multiline
              numberOfLines={4}
              textAlignVertical="top"
              onBlur={onBlur}
              onChangeText={onChange}
              value={value}
              className="w-full h-28 border border-gray-300 p-2 rounded-md font-ops-r text-base text-black"
            />
          )}
          rules={{
            required: "You must supply a description",
            minLength: {
              value: 5,
              message: "Description must be at least 5 characters long"
            }
          }}
        />

        {errors.description && (
          <Text className="text-red-500 mt-1 text-sm">
            {errors.description.message}
          </Text>
        )}

        <SubmitButton
          title="Submit"
          onPress={handleSubmit(onIncidentReportFormSubmit)}
          disabled={!isValid}
        />
      </View>

      <SnackBarMessage
        snackbarVisible={snackbarVisible}
        textMessage={snackbarMessage}
        styleSheet={{
          backgroundColor: "#1976D2",
          position: "absolute",
          bottom: 10
        }}
      />
    </>
  );
}

export default IncidentReportForm;