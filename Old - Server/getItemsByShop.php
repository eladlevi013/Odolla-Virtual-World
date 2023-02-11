<?php
$servername = "localhost";
$name = "id17704470_odollamysql_name";
$password = "?GaLMiroNavESaSoni2312";
$dbname = "id17704470_odollamysql";

// $servername = "localhost";
// $name = "root";
// $password = "";
// $dbname = "unitybackendodolla";

$store = $_POST["store"];
$category = $_POST["category"];

// Create connection
$conn = new mysqli($servername, $name, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT * FROM items WHERE item_shop = '" . $store . "' AND category_name = '". $category . "';";
//$result = $conn->query($sql);
$result = $conn->query($sql);

$rows = [];
while($row = mysqli_fetch_array($result))
{
    $rows[] = $row;
}

$json = "[";
for ($i = 0; $i < count($rows); $i++)
{
	$json .= "{\"item_name\":\"" . $rows[$i][0]. "\",";
	//echo($rows[$i][0]);
	$json .= "\"item_price\":\"" . $rows[$i][1]. "\",";
	//echo($rows[$i][1]);
	$json .= "\"hebrew_name\":\"" . $rows[$i][3]. "\",";
	//echo($rows[$i][3]);
	$json .= "\"category_name\":\"" . $rows[$i][4]. "\"}";
	//echo($rows[$i][4]);
	if(count($rows)-1 != $i) {
		$json .= ",";
	}
}
$json .= "]";

echo($json);
?>