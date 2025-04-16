import cv2 as cv
import matplotlib.pyplot as plt

# Opening webcam
webcam = cv.VideoCapture(0)
if not webcam.isOpened():
    print("ERROR: Cannot open default webcam.")
    exit(1)

# Reading a single frame from webcam
ok, frame = webcam.read()
if not ok:
    print("ERROR: Problem reading frame from webcam.")
    exit(2)

# Display default webcam frame
cv.imshow('frame', frame)
while cv.waitKey(1) != ord('q'):
    pass

# Display greyscaled webcam frame, OpenCV by default has frames in BGR not RGB
greyscale = cv.cvtColor(frame, cv.COLOR_BGR2GRAY)
cv.imshow('grey_frame', greyscale)
while cv.waitKey(1) != ord('q'):
    pass

# Creating our histogram of the greyscale.
#
# Bins (pixel-value grouping): 256
# Range (pixel-values we care about): 0-256
#
# Note: Make sure bins and range parameter are appropriate for each other,
#       otherwise weird results can occur (i.e. bins=256 range=[0, 100]).
#
histogram = cv.calcHist([greyscale], [0], None, [256], [0, 256])

# Plot and show grayscale histogram
plt.figure()
plt.xlabel("Pixel Values (Bins)")
plt.ylabel("# of Pixels")
plt.plot(histogram)
plt.ylim(bottom=0)
plt.xlim([0, 256])
plt.show()

# Clean-up
print("Done. Cleaning up...")
webcam.release()
cv.destroyAllWindows()
