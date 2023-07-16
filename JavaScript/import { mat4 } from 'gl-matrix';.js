import { mat4 } from 'gl-matrix';

// Get a reference to the canvas element
const canvas = document.querySelector("canvas");

// Get the WebGL context
const gl = canvas.getContext("webgl");

// Define the vertex shader source code
const vsSource = `
  attribute vec4 aVertexPosition;

  uniform mat4 uModelViewMatrix;
  uniform mat4 uProjectionMatrix;

  void main() {
    gl_Position = uProjectionMatrix * uModelViewMatrix * aVertexPosition;
  }
`;

// Define the fragment shader source code
const fsSource = `
  precision mediump float;

  void main() {
    gl_FragColor = vec4(1.0, 1.0, 1.0, 1.0);
  }
`;

// Initialize the shader program
const shaderProgram = initShaderProgram(gl, vsSource, fsSource);

// Get the location of the vertex position attribute
const vertexPositionAttributeLocation = gl.getAttribLocation(
  shaderProgram,
  "aVertexPosition"
);

// Create a buffer for the vertex data
const vertexBuffer = gl.createBuffer();

// Bind the vertex buffer
gl.bindBuffer(gl.ARRAY_BUFFER, vertexBuffer);

// Define the vertex data
const vertices = [
  0.0,
  0.0,
  0.0,
  0.0,
  0.5,
  0.0,
  0.5,
  0.0,
  0.0,
  0.0,
  -0.5,
  0.0,
  -0.5,
  0.0,
  0.0,
  0.0,
  0.0,
  -0.5,
  0.0,
  0.0,
  0.5
];

// Copy the vertex data to the buffer
gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(vertices), gl.STATIC_DRAW);

// Create a matrix to hold the model view matrix
const modelViewMatrix = mat4.create();

// Set the position of the ball
mat4.translate(modelViewMatrix, modelViewMatrix, [0.0, 0.0, -5.0]);

// Create a matrix to hold the projection matrix
const projectionMatrix = mat4.create();

// Set the perspective of the projection matrix
mat4.perspective(
  projectionMatrix,
  (45 * Math.PI) / 180,
  canvas.width / canvas.height,
  0.1,
  100.0
);

// Set the viewport
gl.viewport(0, 0, canvas.width, canvas.height);

// Clear the canvas
gl.clearColor(0.0, 0.0, 0.0, 1.0);
gl.clear(gl.COLOR_BUFFER_BIT);

// Use the shader program
gl.useProgram(shaderProgram);

// Enable the vertex position attribute
gl.enableVertexAttribArray(vertexPositionAttributeLocation);

// Bind the vertex buffer
gl.bindBuffer(gl.ARRAY_BUFFER, vertexBuffer);

// Tell WebGL how to interpret the vertex data
gl.vertexAttribPointer(
  vertexPositionAttributeLocation,
  3,
  gl.FLOAT,
  false,
  0,
  0
);

// Set the uniform values
const modelViewMatrixLocation = gl.getUniformLocation(
  shaderProgram,
  "uModelViewMatrix"
);
gl.uniformMatrix4fv(modelViewMatrixLocation, false, modelViewMatrix);

const projectionMatrixLocation = gl.getUniformLocation(
  shaderProgram,
  "uProjectionMatrix"
);
gl.uniformMatrix4fv(projectionMatrixLocation, false, projectionMatrix);

// Draw the ball
gl.drawArrays(gl.TRIANGLE_FAN, 0, 7);

function initShaderProgram(gl, vsSource, fsSource) {
  // Compile the vertex shader
  const vertexShader = gl.createShader(gl.VERTEX_SHADER);
  gl.shaderSource(vertexShader, vsSource);
  gl.compileShader(vertexShader);
  if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
    console.error(
      "An error occurred compiling the vertex shader:",
      gl.getShaderInfoLog(vertexShader)
    );
    return null;
  }

  // Compile the fragment shader
  const fragmentShader = gl.createShader(gl.FRAGMENT_SHADER);
  gl.shaderSource(fragmentShader, fsSource);
  gl.compileShader(fragmentShader);
  if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
    console.error(
      "An error occurred compiling the fragment shader:",
      gl.getShaderInfoLog(fragmentShader)
    );
    return null;
  }

  // Link the shaders into a program
  const shaderProgram = gl.createProgram();
  gl.attachShader(shaderProgram, vertexShader);
  gl.attachShader(shaderProgram, fragmentShader);
  gl.linkProgram(shaderProgram);
  if (!gl.getProgramParameter(shaderProgram, gl.LINK_STATUS)) {
    console.error(
      "An error occurred linking the shader program:",
      gl.getProgramInfoLog(shaderProgram)
    );
    return null;
  }

  return shaderProgram;
}
