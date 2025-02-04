/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      colors: {
        'primary': '#fff0f5',
        'secondary': '#ffffff',
        'accent': '#4a4a4a',
        'tprimary': '#ff4d8d',
        'tsecondary': '#fb659c',
      },
    },
  },
  plugins: [],
};
