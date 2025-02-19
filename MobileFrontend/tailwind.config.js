/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./app/**/*.{js,jsx,ts,tsx}"],
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
        primary: "#2196F3",  // Dark Blue
        secondary: "#42A5F5", // Light Blue
        light: "#BBDEFB",    // Very Light Blue
      },
    },
  },
  plugins: [],
};
