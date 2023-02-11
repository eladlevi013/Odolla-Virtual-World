<?php
$servername = "localhost";
$name = "id17704470_odollamysql_name";
$password = "?GaLMiroNavESaSoni2312";
$dbname = "id17704470_odollamysql";

$loginUser = $_POST["loginUser"];

// Create connection
$conn = new mysqli($servername, $name, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT level FROM users WHERE name = '" . $loginUser . "';";

$result = $conn->query($sql);

$temp = $result->fetch_array()[0];
echo $temp;
?>