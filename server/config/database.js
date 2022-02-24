const { Sequelize } = require("sequelize");
const sequelize = new Sequelize(
  process.env.DB_NAME,
  process.env.DB_USR,
  process.env.DB_PASS,
  {
    host: process.env.SERVER,
    dialect: "mysql",
    logging: false,
    pool: {
      max: 5,
      min: 0,
      acquire: 30000,
      idle: 10000,
    },
  }
);

sequelize.sync({ alter: true }).catch((err) => console.log("Error: " + err));

module.exports = sequelize;
