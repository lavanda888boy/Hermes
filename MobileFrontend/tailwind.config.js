/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./app/**/*.{js,jsx,ts,tsx}",
    "./components/**/**/*.{js,jsx,ts,tsx}"
  ],
  presets: [require("nativewind/preset")],
  theme: {
    extend: {
      fontFamily: {
        "ops-it": ["OpenSans-Italic"],
        "ops-l": ["OpenSans-Light"],
        "ops-r": ["OpenSans-Regular"],
        "ops-sb": ["OpenSans-SemiBold"],
      },
      colors: {
        primary: "#1976D2",  // Dark Blue
        secondary: "#2196F3", // Light Blue
        light: "#42A5F5",    // Very Light Blue
      },
    },
  },
  plugins: [],
};
