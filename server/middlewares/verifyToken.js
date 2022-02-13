const { jwtVerify } = require("./jwtUtil");

const verifyToken = async (req, res, next) => {
  try {
    const authHeader = req.headers["authorization"];
    const token = authHeader.split(" ")[1];

    if (!token) return res.status(401).json({ error_message: "Unauthorized!" });

    const payload = jwtVerify(token, process.env.JWT_ACC_TOKEN_KEY);

    if (!payload)
      return res.status(401).json({ error_message: "Unauthorized!" });

    req.user = payload;
  } catch (err) {
    return res.status(401).json({ error_message: err.message });
  }
  next();
};



const verifyTokenAndAdmin = (req, res, next) => {
  verifyToken(req, res, () => {
    if (req.user.isAdmin) {
      next();
    } else {
      res.status(403).json({ error_message: "You are not alowed to do that!" });
    }
  });
};

module.exports = {
  verifyToken,
  verifyTokenAndAdmin,
};