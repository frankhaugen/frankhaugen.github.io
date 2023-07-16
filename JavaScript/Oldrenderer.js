function createCanvas(width, height) {
    const canvas = document.createElement("canvas");
    canvas.width = width;
    canvas.height = height;
    document.body.appendChild(canvas);
    return canvas;
}

function getGLContext(canvas) {
    const gl = canvas.getContext("webgl");
    gl.clearColor(0, 0.75, 0.75, 0.75); // set clear color to black
    return gl;
}

function createShader(gl, type, source) {
    const shader = gl.createShader(type);
    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    return shader;
}

function createProgram(gl, vertexShader, fragmentShader) {
    const program = gl.createProgram();
    gl.attachShader(program, vertexShader);
    gl.attachShader(program, fragmentShader);
    gl.linkProgram(program);
    return program;
}

function createBuffer(gl, data, bufferType, drawType) {
    const buffer = gl.createBuffer();
    gl.bindBuffer(bufferType, buffer);
    gl.bufferData(bufferType, new Float32Array(data), drawType);
    return buffer;
}

function computeVertices(vectors) {
    let vertices = [];

    for (let vector of vectors) {
        vertices.push(vector.x, vector.y, vector.z);
    }

    return new Float32Array(vertices);
}

function computeIndices(vectors) {
    let indices = [];

    for (let i = 0; i < vectors.length; i++) {
        indices.push(i);
    }

    return new Uint16Array(indices);
}

function computeWidth(vectors) {
    let minX = Number.MAX_VALUE;
    let maxX = Number.MIN_VALUE;

    for (let vector of vectors) {
        if (vector.x < minX) {
            minX = vector.x;
        }

        if (vector.x > maxX) {
            maxX = vector.x;
        }
    }

    var width = maxX - minX;
    if (width < 100) {
        width = 100;
    }

    return width;
}

function computeHeight(vectors) {
    let minY = Number.MAX_VALUE;
    let maxY = Number.MIN_VALUE;

    for (let vector of vectors) {
        if (vector.y < minY) {
            minY = vector.y;
        }

        if (vector.y > maxY) {
            maxY = vector.y;
        }
    }

    var height = maxY - minY;

    if (height < 100) {
        height = 100;
    }

    return height;
}

class Vector3X {
    constructor(x, y, z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

function createVectors(width, height, depth) {
    let vectors = [];

    for (let x = 0; x < width; x++) {
        for (let y = 0; y < height; y++) {
            vectors.push(new Vector3(x, y, depth));
        }
    }

    return vectors;
}

// Assuming that the `vertices` argument is an array of numbers
function draw(gl, program, vertices, indices) {
    const positionBuffer = createBuffer(
        gl,
        vertices,
        gl.ARRAY_BUFFER,
        gl.STATIC_DRAW
    );
    const indexBuffer = createBuffer(
        gl,
        indices,
        gl.ELEMENT_ARRAY_BUFFER,
        gl.STATIC_DRAW
    );
    const color = [1.0, 0.0, 0.0, 1.0]; // red
    const matrix = new Float32Array([
        1,
        0,
        0,
        0,
        0,
        1,
        0,
        0,
        0,
        0,
        1,
        0,
        0,
        0,
        0,
        1
    ]);
    const positionAttributeLocation = gl.getAttribLocation(program, "position");
    const matrixUniformLocation = gl.getUniformLocation(program, "matrix");
    const colorUniformLocation = gl.getUniformLocation(program, "color");

    console.log("start draw");
    console.log(positionAttributeLocation);
    console.log(matrixUniformLocation);
    console.log(colorUniformLocation);

    gl.uniform4fv(colorUniformLocation, color);
    gl.uniformMatrix4fv(matrixUniformLocation, false, matrix);
    gl.enableVertexAttribArray(positionAttributeLocation);
    gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
    gl.vertexAttribPointer(positionAttributeLocation, 3, gl.FLOAT, false, 0, 0);

    gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, indexBuffer);
    gl.drawElements(gl.TRIANGLES, indices.length, gl.UNSIGNED_SHORT, 0);

    console.log(gl.getParameter(gl.ARRAY_BUFFER_BINDING));
    console.log(gl.getParameter(gl.ELEMENT_ARRAY_BUFFER_BINDING));
    console.log(
        gl.getVertexAttrib(
            positionAttributeLocation,
            gl.VERTEX_ATTRIB_ARRAY_ENABLED
        )
    );
    console.log("end draw");
}

function renderX(vectors) {
    console.debug("rendering");
    const canvas = createCanvas(computeWidth(vectors), computeHeight(vectors));
    const gl = getGLContext(canvas);

    console.debug("creating shaders");
    const vertexShaderSource = `
attribute vec3 position;
uniform mat4 matrix;
void main() {
  gl_Position = matrix * vec4(position, 1);
}
`;
    const fragmentShaderSource = `
precision highp float;
uniform vec4 color;
void main() {
  gl_FragColor = color;
}
`;

    console.debug("creating program");
    const vertexShader = createShader(gl, gl.VERTEX_SHADER, vertexShaderSource);
    const fragmentShader = createShader(
        gl,
        gl.FRAGMENT_SHADER,
        fragmentShaderSource
    );
    const program = createProgram(gl, vertexShader, fragmentShader);

    console.debug("drawing");
    gl.useProgram(program);

    const indices = computeIndices(vectors);
    const vertices = computeVertices(vectors);

    if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
        console.error(gl.getShaderInfoLog(vertexShader));
    }

    if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
        console.error(gl.getShaderInfoLog(fragmentShader));
    }

    if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error(gl.getProgramInfoLog(program));
    }

    draw(gl, program, vertices, indices);

    console.debug("done");
}

function createCanvas(width, height) {
    const canvas = document.createElement("canvas");
    canvas.width = width;
    canvas.height = height;
    return canvas;
}

function getGLContext(canvas) {
    const gl = canvas.getContext("webgl");
    return gl;
}

function createShader(gl, type, source) {
    const shader = gl.createShader(type);
    gl.shaderSource(shader, source);
    gl.compileShader(shader);
    return shader;
}

function createProgram(gl, vertexShader, fragmentShader) {
    const program = gl.createProgram();
    gl.attachShader(program, vertexShader);
    gl.attachShader(program, fragmentShader);
    gl.linkProgram(program);
    return program;
}

function createBuffer(gl, data, bufferType, drawType) {
    const buffer = gl.createBuffer();
    gl.bindBuffer(bufferType, buffer);
    gl.bufferData(bufferType, data, drawType);
    return buffer;
}

function computeVertices(vectors) {
    const vertices = [];
    for (let i = 0; i < vectors.length; i++) {
        vertices.push(vectors[i].x, vectors[i].y, vectors[i].z);
    }
    return new Float32Array(vertices);
}

function computeIndices(vectors) {
    const indices = [];
    for (let i = 0; i < vectors.length; i += 4) {
        indices.push(i, i + 1, i + 2, i + 2, i + 1, i + 3);
    }
    return new Uint16Array(indices);
}

function computeWidth(vectors) {
    return vectors[1].x - vectors[0].x;
}

function computeHeight(vectors) {
    return vectors[2].y - vectors[0].y;
}

class Vector3 {
    constructor(x, y, z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

function createVectors(width, height, depth) {
    const vectors = [];
    for (let i = 0; i < 4; i++) {
        const x = i % 2 === 0 ? 0 : width;
        const y = i < 2 ? 0 : height;
        const z = i < 2 ? depth : 0;
        vectors.push(new Vector3(x, y, z));
    }
    return vectors;
}

function draw(gl, program, vertices, indices) {
    const positionAttributeLocation = gl.getAttribLocation(program, "position");
    const matrixUniformLocation = gl.getUniformLocation(program, "matrix");
    const colorUniformLocation = gl.getUniformLocation(program, "color");

    const positionBuffer = createBuffer(gl, vertices, gl.ARRAY_BUFFER, gl.STATIC_DRAW);
    gl.enableVertexAttribArray(positionAttributeLocation);
    gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
    gl.vertexAttribPointer(positionAttributeLocation, 3, gl.FLOAT, false, 0, 0);

    const indexBuffer = createBuffer(gl, indices, gl.ELEMENT_ARRAY_BUFFER, gl.STATIC_DRAW);

    gl.uniformMatrix4fv(matrixUniformLocation, false, new Float32Array([
        2 / gl.canvas.width, 0, 0, 0,
        0, -2 / gl.canvas.height, 0, 0,
        0, 0, 1, 0,
        -1, 1, 0, 1,
    ]));

    gl.uniform4fv(colorUniformLocation, [1, 0, 0, 1]);

    gl.drawElements(gl.TRIANGLES, indices.length, gl.UNSIGNED_SHORT, 0);
}

function render(vectors) {
    console.debug("rendering");
    const canvas = createCanvas(computeWidth(vectors), computeHeight(vectors));
    const gl = getGLContext(canvas);

    console.debug("creating shaders");
    const vertexShaderSource = `
attribute vec3 position;
uniform mat4 matrix;
void main() {
    gl_Position = matrix * vec4(position, 1);
}
`;
    const fragmentShaderSource = `
precision highp float;
uniform vec4 color;
void main() {
    gl_FragColor = color;
}
`;

    console.debug("creating program");
    const vertexShader = createShader(gl, gl.VERTEX_SHADER, vertexShaderSource);
    const fragmentShader = createShader(gl, gl.FRAGMENT_SHADER, fragmentShaderSource);
    const program = createProgram(gl, vertexShader, fragmentShader);

    console.debug("drawing");
    gl.useProgram(program);

    const indices = computeIndices(vectors);
    const vertices = computeVertices(vectors);

    console.debug("vertices:");
    console.debug(vertices);
    console.debug("indices:");
    console.debug(indices);

    if (!gl.getShaderParameter(vertexShader, gl.COMPILE_STATUS)) {
        console.error(gl.getShaderInfoLog(vertexShader));
    }

    if (!gl.getShaderParameter(fragmentShader, gl.COMPILE_STATUS)) {
        console.error(gl.getShaderInfoLog(fragmentShader));
    }

    if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error(gl.getProgramInfoLog(program));
    }

    draw(gl, program, vertices, indices);

    console.debug("done");
}

// Test Case 1
const vectors1 = createVectors(2, 2, 2);
render(vectors1);

// Test Case 2
const vectors2 = createVectors(3, 3, 3);
render(vectors2);

// Test Case 3
const vectors3 = createVectors(4, 4, 4);
render(vectors3);