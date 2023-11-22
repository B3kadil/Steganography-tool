Steganography Tool
Overview
Steganography Tool is a command-line utility created for embedding and extracting hidden messages using the art of steganography. Steganography involves concealing information within other data, and this tool allows users to experiment with this technique in a user-friendly way. It supports encoding messages into text containers and decoding them back.

How to Use
Embedding a Message
Open a command prompt or terminal
Navigate to the directory containing the Steganography Tool.
Use the following command to embed a message:

dotnet build -o out

to encrypt
dotnet Steganography.dll --message "path/to/message.txt" --stego "path/to/stegocontainer.txt" --container "path/to/container.txt"


to decrypt
dotnet Steganography.dll --stego "path/to/stegocontainer.txt" --message "path/to/extracted_message.txt"

to help
dotnet Steganography.dll -h

