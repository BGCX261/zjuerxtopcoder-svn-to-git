<?php
	$fpath='tmp/';
	if(file_exists($fpath.'a.so'))
		unlink($fpath.'a.so');
	move_uploaded_file($_FILES['file']['tmp_name'],$fpath.'a.cpp');
	system("g++ -fpic -shared -o tmp/a.so tmp/a.cpp");
	system("./a.out >> tmp/log.txt");
	if(file_exists($fpath.'a.cpp'))
		unlink($fpath.'a.cpp');
	if(file_exists($fpath.'a.so'))
	{
		echo '<br>DONE!</br>';
		echo "<a href=tmp/a.so>click here</a> to download a.so<br />";
	}
	else
	{
		echo '<br>Oooops! CE, you know it.</br>';
	}
?>
