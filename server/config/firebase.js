// Import the functions you need from the SDKs you need
const { initializeApp } = require("firebase/app");
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
const firebaseConfig = {
  apiKey: "AIzaSyDPfjoArflbw-pWjQHnV6M5kBHwqX6KNTo",
  authDomain: "armydrones-4a80f.firebaseapp.com",
  projectId: "armydrones-4a80f",
  storageBucket: "armydrones-4a80f.appspot.com",
  messagingSenderId: "972003070239",
  appId: "1:972003070239:web:c6ae1ef1784022bcefa02a",
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

module.exports = app;
