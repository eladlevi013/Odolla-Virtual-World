<?php
$servername = "localhost";
$name = "id17704470_odollamysql_name";
$password = "?GaLMiroNavESaSoni2312";
$dbname = "id17704470_odollamysql";

// $servername = "localhost";
// $name = "root";
// $password = "";
// $dbname = "unitybackendodolla";

$username = $_POST["username"];
$category = $_POST["category"];
$ItemListByCategory = $_POST["ItemListByCategory"];
$money = $_POST["money"];

// Create connection
$conn = new mysqli($servername, $name, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "UPDATE users SET coins = coins - " . $money . " WHERE name = '" . $username . "';";
$result = $conn->query($sql);
echo($result);

$sql = "UPDATE users SET " . $category . " = '" . $ItemListByCategory . "' WHERE name = '" . $username . "';";
$result = $conn->query($sql);
echo($result);

echo $result;
?>