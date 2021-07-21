<?php
	$servername = "122.32.165.55";
	$server_username = "team";
	$server_password = "abcd1234";
	$dbname = "coex";

	$musicTitle = $_POST["musicPost"]; //검색할 곡 명
	$musicLevel = $_POST["levelPost"]; //검색할 곡 난이도
	
	$conn = new mysqli($servername, $server_username, $server_password, $dbname);
	
	if(!$conn)
	{
		die("Connection Failed.". mysqli_connect_error());
	}
	
	$sql = "select musicnum from musiclevel WHERE level = '".$musicLevel."' AND title = '".$musicTitle."'"; 
	$result = mysqli_query($conn, $sql);
		
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){
			echo "".$row['musicnum'];
		}
	}else {
		echo "musiclevel not found";
	}
	
?>
