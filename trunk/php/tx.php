<?php
 
$im = imagecreatefrompng("nana.png");
$num = rand(0, 3);
if ($num == 0) {
        $im = imagecreatefrompng("yuri.png");
}
else if ($num == 1) {
        $im = imagecreatefrompng("nana.png");
}
else if ($num == 2) {
        $im = imagecreatefrompng("shu.png");
}
else {
        $im = imagecreatefrompng("bing.png");
}
imagesavealpha($im, true);
 
// output image
header("Content-Type: image/png");
imagepng($im);
imagedestroy($im);
 
?>

