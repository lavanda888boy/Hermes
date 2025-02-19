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
    },
  },
  plugins: [],
};
