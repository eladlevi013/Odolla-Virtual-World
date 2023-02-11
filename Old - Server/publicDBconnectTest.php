<?php
$servername = "localhost";
$username = "id17704470_odollamysql_name";
$password = "?GaLMiroNavESaSoni2312";
$dbname = "id17704470_odollamysql";



$conn = new mysqli($servername, $username, $password, $dbname);



// Check connection

if ($conn->connect_error) {

  die("Connection failed: " . $conn->connect_error);

}



echo "heyyyyyyyyyyyyyyyyyy";

?>