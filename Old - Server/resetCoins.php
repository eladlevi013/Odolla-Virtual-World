<?php
$servername = "localhost";
$name = "id17704470_odollamysql_name";
$password = "?GaLMiroNavESaSoni2312";
$dbname = "id17704470_odollamysql";

$username = $_POST["username"];

// Create connection
$conn = new mysqli($servername, $name, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

// $sql = "SELECT coins FROM users WHERE name = '" . $loginUser . "';";

# UPDATE users SET coins = coins + 10 WHERE name = 'elad'
$sql = "UPDATE users SET coins = 0 WHERE name = '" . $username . "';";
$result = $conn->query($sql);
echo $sql;

#$temp = $result->fetch_array()[0];
echo $result;
?>