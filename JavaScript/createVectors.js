import { Vector3 } from "./Vector3";

export class VectorUtils {
  static createVectors(width, height, depth) {
    let vectors = [];

    for (let x = 0; x < width; x++) {
      for (let y = 0; y < height; y++) {
        vectors.push(new Vector3(x, y, depth));
      }
    }

    return vectors;
  }

  static computeVertices(vectors) {
    const vertices = [];
    for (let i = 0; i < vectors.length; i++) {
      vertices.push(vectors[i].x, vectors[i].y, vectors[i].z);
    }
    return new Float32Array(vertices);
  }

  static computeIndices(vectors) {
    const indices = [];
    for (let i = 0; i < vectors.length; i += 4) {
      indices.push(i, i + 1, i + 2, i + 2, i + 1, i + 3);
    }
    return new Uint16Array(indices);
  }
}