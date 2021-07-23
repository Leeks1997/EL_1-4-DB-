<?php
	$servername = "122.32.165.55";
	$server_username = "team";
	$server_password = "abcd1234";
	$dbname = "coex";
	
	$cmd = $_POST["cmd"]; //실행할 쿼리 구문
	$cmdNum = $_POST["numPost"]; //사용할 인자 값
	
	$conn = new mysqli($servername, $server_username, $server_password, $dbname);
	
	if(!$conn)
	{
		die("Connection Failed.". mysqli_connect_error());
	}

	if($cmd == "all"){
		//cmdNum의 사용자 정보 조회
		$sql = "select username, score from player WHERE musicnum = ".$cmdNum." order by score desc"; 
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
	}else if($cmd == "rank"){
		//cmdNum의 랭킹
		$sql = "select COUNT(*)+1 AS rank from player WHERE score >= ".$cmdNum; 
		$result = mysqli_query($conn, $sql);

		if(mysqli_num_rows($result) > 0){
			while($row = mysqli_fetch_assoc($result)){
				echo $row['rank'];
			}
		}else {
			echo "rank not found";
		}
	}

?>
