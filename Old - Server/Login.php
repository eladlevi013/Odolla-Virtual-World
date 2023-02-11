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

$loginPass = md5($loginPass);

// Create connection
$conn = new mysqli($servername, $name, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT password FROM users WHERE name = '" . $loginUser . "';";
$result = $conn->query($sql);

if($result->num_rows > 0){
	while($row = $result->fetch_assoc()){
		if($row["password"] == $loginPass){

			$sql = "SELECT InBan FROM users WHERE name = '" . $loginUser . "';";
			$result = $conn->query($sql);

			while($row = $result->fetch_assoc()){
				if($row["InBan"] == 0){
					echo "Login Success.";
					// Get user's data here.
					// Get player info.
					// Get Inventory 
					// Modify player data.
					// Update inventory.

					// Update the last login date:
					$milliseconds = round(microtime(true) * 1000);
					$sql = "UPDATE users SET lastLoginDate = " . $milliseconds . " WHERE name = '" . $loginUser . "';";
					$conn->query($sql);
					
					$time =  "[" . date("d/m/y") . ", " . date("h:i") . "] ";
                    $message = $time . " the user: " . $loginUser . " logged in";
                    file_put_contents("log.txt", "$message\n", FILE_APPEND);
				}

				else {
					echo "Player Banned.";
				}
			}
		}

		else {
			echo "Wrong Credentials.";
		}
	}
} else {
	echo "name does not exists.";
}
?>