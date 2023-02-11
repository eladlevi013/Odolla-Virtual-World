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

$sql = "SELECT curr_outfit FROM users WHERE name = '" . $username . "';";
$result = $conn->query($sql);

$temp = $result->fetch_array()[0];
echo $temp;
?>