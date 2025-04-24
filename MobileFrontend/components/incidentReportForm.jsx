import { Controller, useForm } from "react-hook-form";
import { View, Text, TextInput } from "react-native";
import SubmitButton from "./submitButton";

const IncidentReportForm = () => {
  const { control, handleSubmit, formState: { errors } } = useForm();

  return (
    <View className="flex justify-center p-6 rounded-2xl elevation-md bg-white">
      <Text className="text-4xl text-center text-primary font-ops-sb">
        New incident
      </Text>

      <Controller
        control={control}
        name="description"
        render={({ field }) => (
          <TextInput
            {...field}
            className="w-full"
          />
        )}
        rules={{ required: true, minLength: 5 }}
      />

      <SubmitButton
        title="Submit"
        onPress={handleSubmit((data) => console.log(data))}
      // disabled={errors.description ? true : false}
      />
    </View>
  );
}

export default IncidentReportForm;