<?php
$servername = "localhost";
$name = "id17704470_odollamysql_name";
$password = "?GaLMiroNavESaSoni2312";
$dbname = "id17704470_odollamysql";
// $servername = "localhost";
// $name = "root";
// $password = "";
// $dbname = "unitybackendodolla";

$ban = $_POST["ban"];
$username = $_POST["username"];

// Create connection
$conn = new mysqli($servername, $name, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "UPDATE users SET InBan = " . $ban . " WHERE name = '" . $username . "';";
$result = $conn->query($sql);
echo $result;

if($ban == 1) {
    $time =  "[" . date("d/m/y") . ", " . date("h:i") . "] ";
    $message = $time . " The user: " . $username . " banned";
    file_put_contents("log.txt", "$message\n", FILE_APPEND);
}
else {
    $time =  "[" . date("d/m/y") . ", " . date("h:i") . "] ";
    $message = $time . " The user: " . $username . " un-banned";
    file_put_contents("log.txt", "$message\n", FILE_APPEND);
}
?>