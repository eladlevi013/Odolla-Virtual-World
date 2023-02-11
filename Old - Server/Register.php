<?php
$servername = "localhost";
$name = "id17704470_odollamysql_name";
$password = "?GaLMiroNavESaSoni2312";
$dbname = "id17704470_odollamysql";

// $servername = "localhost";
// $name = "root";
// $password = "";
// $dbname = "unitybackendodolla";

$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];
$email = $_POST["email"];

$loginPass = md5($loginPass);

// Create connection
$conn = new mysqli($servername, $name, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT name FROM users WHERE name = '" . $loginUser . "';";

$result = $conn->query($sql);

if($result->num_rows > 0){
	// Tell user that theat name is already taken
	echo "User is already taken.";
} else {
	echo "Creating user...";
	// Insert the user and password into the database
	$sql = "INSERT INTO users (name, password, coins, email) VALUES ('" . $loginUser . "', '" . $loginPass . "'," . 0 . ", '" . $email . "')";
	if ($conn->query($sql) == TRUE) {
		echo "New record created successfully";
		
		$time =  "[" . date("d/m/y") . ", " . date("h:i") . "] ";
        $message = $time . " the user: " . $loginUser . " created";
        file_put_contents("log.txt", "$message\n", FILE_APPEND);
	} else {
		echo "Error: " . $sql . "<br>" , $conn->error;
	}
}
?>