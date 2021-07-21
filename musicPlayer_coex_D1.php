<?php
	$servername = "122.32.165.55";
	$server_username = "team";
	$server_password = "abcd1234";
	$dbname = "coex";

	$musicNum = $_POST["numPost"]; //검색할 곡 번호
	
	$conn = new mysqli($servername, $server_username, $server_password, $dbname);
	
	if(!$conn)
	{
		die("Connection Failed.". mysqli_connect_error());
	}
	
	$sql = "select username, score from player WHERE musicnum = ".$musicNum." order by score desc"; 
	$result = mysqli_query($conn, $sql);
		
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){
			echo "|username:" .$row['username'];
			echo "|score:" .$row['score'];
			echo ";";
		}
	}else {
		echo "musiclevel not found";
	}
?>
