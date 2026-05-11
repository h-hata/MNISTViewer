# MNISTViewer
Dump tool for the MNIST image files
This tool opens MNIST the image file ans the label file,
and show you the byte data at cell in 28x28pics.

The MNIST train data and test data consist of label file and image file respectibily.
The label data leads 8 byte header and 1 byte labels for each image follow. The image data
leads 16 byte header and 28x28 bytes image datas for each image.

The train files contain 60000 images, so the size of the label file,
train-labels-idx1-ubyte is 60,008 bytes,
and the image file, train-images-idx3-ubyte contains 47,040,016 bytes.

The test files contain 10000 images, so the size of the label file,
t10k-labels-idx1-ubyte is 10,008 bytes,
and the image file, t10k-images-idx3-ubyte contains 7,840,016 bytes.

This tool reads either test or train files and show image data and lavel
for each image.

