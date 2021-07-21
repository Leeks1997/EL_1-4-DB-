<?php
	$servername = "122.32.165.55";
	$server_username = "team";
	$server_password = "abcd1234";
	$dbname = "coex";
	
	$conn = new mysqli($servername, $server_username, $server_password, $dbname);
	
	if(!$conn)
	{
		die("Connection Failed.". mysqli_connect_error());
	}
	
	$sql = "select * from music"; //모든 음악 리스트 출력 및 웹상으로 정보 띄우기
	$result = mysqli_query($conn, $sql);
		
	if(mysqli_num_rows($result) > 0){
		while($row = mysqli_fetch_assoc($result)){
			echo "|title:" .$row['title'];
			echo "|composer:" .$row['composer'];
			echo "|runtime:" .$row['runtime'];
			echo "|genre:" .$row['genre'];
			echo "|bpm:" .$row['bpm'];
			echo ";";
		}
	}else {
		echo "music not found";
	}
?>
