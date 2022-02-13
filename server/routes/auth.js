const router = require("express").Router();
// const User = require("../models/User");
const bcrypt = require("bcrypt");
const {
  createAccessToken,
  jwtVerify,
} = require("../middlewares/jwtUtil");

router.post("/register", async (req, res) => {
  const userData = req.body;


res.status(201).json({ user: "createdUser" });
});

router.post("/login", async (req, res) => {

  res.status(200).json({
    user: "foundUser",
    accessToken: accessToken,
  });

});


module.exports = router;