using Godot;
using System.Collections.Generic;

namespace Utilities.Debug
{
    public class Gizmos : Spatial
    {
        private struct ShapeData {
            public Vector3 position;
            public Vector3 scale;
            public Color color;
        }

        private struct MultimeshData {
            public RID MultimeshRID;
            public ArrayMesh Mesh;
            public List<ShapeData> DebugGeometry;

            public MultimeshData(RID multimeshRID, ArrayMesh mesh, List<ShapeData> debugGeometry) {
                this.MultimeshRID = multimeshRID;
                this.Mesh = mesh;
                this.DebugGeometry = debugGeometry;
            }
        }

        private enum Shapes {
            Sphere,
            WireSphere,
            Line,
            Box,
            WireBox,
            Point,
            Capsule,
            WireCapsule,
        }

        private static Dictionary<Shapes, MultimeshData> geometry;
        private SpatialMaterial material;


        public override void _Ready() {
            material = new SpatialMaterial();
            material.FlagsUnshaded = true;
            material.FlagsTransparent = true;
            material.VertexColorUseAsAlbedo = true;

            geometry = new Dictionary<Shapes, MultimeshData>()
            {
                {Shapes.Sphere,       CreateDebugGeometryData(CreateSphereVertices(),     Mesh.PrimitiveType.Triangles)},
                {Shapes.WireSphere,   CreateDebugGeometryData(CreateSphereVertices(),     Mesh.PrimitiveType.LineLoop)},
                {Shapes.Line,         CreateDebugGeometryData(CreateLineVertices(),       Mesh.PrimitiveType.Lines)},
                {Shapes.Box,          CreateDebugGeometryData(CreateBoxVertices(),        Mesh.PrimitiveType.Triangles)},
                {Shapes.WireBox,      CreateDebugGeometryData(CreateWireBoxVertices(),    Mesh.PrimitiveType.Lines)},
                {Shapes.Point,        CreateDebugGeometryData(CreatePointVertices(),      Mesh.PrimitiveType.Points)},
                {Shapes.Capsule,      CreateDebugGeometryData(CreateCapsuleVertices(),    Mesh.PrimitiveType.Triangles)},
                {Shapes.WireCapsule,  CreateDebugGeometryData(CreateCapsuleVertices(),    Mesh.PrimitiveType.LineLoop)},
            };
        }

        public override void _Process(float delta) {
            foreach (MultimeshData data in geometry.Values) { 

                var MultimeshRID = data.MultimeshRID;
                var DebugGeometry = data.DebugGeometry;      

                if(VisualServer.MultimeshGetInstanceCount(MultimeshRID) != DebugGeometry.Count) {
                    VisualServer.MultimeshAllocate(
                        MultimeshRID, 
                        DebugGeometry.Count > 0 ? DebugGeometry.Count : 0, 
                        VisualServer.MultimeshTransformFormat.Transform3d, 
                        VisualServer.MultimeshColorFormat.Float);
                }

                for (int i = 0; i < DebugGeometry.Count; i++) {
                    ShapeData geometry = DebugGeometry[i];

                    Transform t = Transform.Translated(geometry.position);
                    t.basis = t.basis.Scaled(geometry.scale);

                    VisualServer.MultimeshInstanceSetTransform(MultimeshRID, i, t);
                    VisualServer.MultimeshInstanceSetColor(MultimeshRID, i, geometry.color);
                }
                DebugGeometry.Clear();
            }
        }

        private RID CreateMultiMesh(ArrayMesh mesh) {
            RID multimesh = VisualServer.MultimeshCreate();
            RID instance = VisualServer.InstanceCreate2(multimesh, GetWorld().Scenario);

            VisualServer.InstanceSetTransform(instance, new Transform (Transform.basis, Vector3.Zero));
            VisualServer.InstanceGeometrySetMaterialOverride(instance, material.GetRid());
            VisualServer.MultimeshSetMesh(multimesh, mesh.GetRid());

            return multimesh;
        }

        private MultimeshData CreateDebugGeometryData(Vector3[] vertices, Mesh.PrimitiveType primitiveType) {
            ArrayMesh mesh = new ArrayMesh();
            Godot.Collections.Array meshData = new Godot.Collections.Array();
            meshData.Resize((int) ArrayMesh.ArrayType.Max);
            meshData[0] = vertices;

            mesh.AddSurfaceFromArrays(primitiveType, meshData);
            RID multimeshRID = CreateMultiMesh(mesh);
            meshData.Dispose();

            return new MultimeshData(multimeshRID, mesh, new List<ShapeData>());
        }

         private Vector3[] CreateSphereVertices() {
            SphereMesh sphere = new SphereMesh();
            sphere.Rings = 8;
            sphere.RadialSegments = 8;

            Vector3[] vertices = sphere.GetFaces();
            sphere.Dispose();

            return vertices;
        }

        private Vector3[] CreateLineVertices() {
            return new Vector3[] { Vector3.Zero, Vector3.One };
        }

        private Vector3[] CreateBoxVertices() {
            CubeMesh cubeMesh = new CubeMesh();
            Vector3[] vertices = cubeMesh.GetFaces();

            for (int i = 0; i < vertices.Length; i++) {
                vertices[i] -= new Vector3(0.5f, 0.5f, 0.5f);
            }
            cubeMesh.Dispose();
            return vertices;
        }

        private Vector3[] CreateWireBoxVertices() {
            Vector3 correction = new Vector3(0.5f, 0.5f, 0.5f);

            return new Vector3[] {
            	new Vector3(0, 0, 0) - correction,
                new Vector3(1, 0, 0) - correction,
                new Vector3(1, 0, 0) - correction,
                new Vector3(1, 0, 1) - correction,
                new Vector3(1, 0, 1) - correction,
                new Vector3(0, 0, 1) - correction,
                new Vector3(0, 0, 1) - correction,
                new Vector3(0, 0, 0) - correction,

               	new Vector3(0, 1, 0) - correction,
                new Vector3(1, 1, 0) - correction,
                new Vector3(1, 1, 0) - correction,
                new Vector3(1, 1, 1) - correction,
                new Vector3(1, 1, 1) - correction,
                new Vector3(0, 1, 1) - correction,
                new Vector3(0, 1, 1) - correction,
                new Vector3(0, 1, 0) - correction,

                new Vector3(0, 0, 0) - correction,
                new Vector3(0, 1, 0) - correction,
                new Vector3(1, 0, 0) - correction,
                new Vector3(1, 1, 0) - correction,
                new Vector3(1, 0, 1) - correction,
                new Vector3(1, 1, 1) - correction,
                new Vector3(0, 0, 1) - correction,
                new Vector3(0, 1, 1) - correction,
                };
        }

        private Vector3[] CreatePointVertices() {
            return new Vector3[] { Vector3.Zero };
        }

        private Vector3[] CreateCapsuleVertices() {
            CapsuleMesh capsuleMesh = new CapsuleMesh();
            capsuleMesh.Rings = capsuleMesh.RadialSegments = 8;

            Vector3[] vertices = capsuleMesh.GetFaces();
            capsuleMesh.Dispose();
            return vertices;
        }

        public static void DrawSphere(Vector3 position, Vector3 scale, Color color) {
            ShapeData sphere = new ShapeData();

            sphere.position = position;
            sphere.scale = scale;
            sphere.color = color;

            geometry[Shapes.Sphere].DebugGeometry.Add(sphere);
        }

        public static void DrawWireSphere(Vector3 position, float radius, Color color) {
            ShapeData wireSphere = new ShapeData();

            wireSphere.position = position;
            wireSphere.scale = new Vector3(radius, radius, radius);
            wireSphere.color = color;

            geometry[Shapes.WireSphere].DebugGeometry.Add(wireSphere);
        }

        public static void DrawLine(Vector3 position, Vector3 direction, Color color) {
            ShapeData line = new ShapeData();

            line.position = position;
            line.scale = direction;
            line.color = color;

            geometry[Shapes.Line].DebugGeometry.Add(line);
        }

        public static void DrawBox(Vector3 position, Vector3 scale, Color color) {
            ShapeData box = new ShapeData();

            box.position = position;
            box.scale = scale;
            box.color = color;

            geometry[Shapes.Box].DebugGeometry.Add(box);
        }

        public static void DrawWireBox(Vector3 position, Vector3 scale, Color color) {
            ShapeData wireBox = new ShapeData();

            wireBox.position = position;
            wireBox.scale = scale;
            wireBox.color = color;

            geometry[Shapes.WireBox].DebugGeometry.Add(wireBox);
        }

        public static void DrawPoint(Vector3 position, Color color) {
            ShapeData wireBox = new ShapeData();

            wireBox.position = position;
            wireBox.scale = Vector3.One;
            wireBox.color = color;

            geometry[Shapes.Point].DebugGeometry.Add(wireBox);
        }

        public static void DrawCapsule(Vector3 position, Vector3 scale, Color color){
            ShapeData capsule = new ShapeData();

            capsule.position = position;
            capsule.scale = scale;
            capsule.color = color;

            geometry[Shapes.Capsule].DebugGeometry.Add(capsule);
        }

        public static void DrawWireCapsule(Vector3 position, Vector3 scale, Color color){
            ShapeData wireCapsule = new ShapeData();

            wireCapsule.position = position;
            wireCapsule.scale = scale;
            wireCapsule.color = color;

            geometry[Shapes.WireCapsule].DebugGeometry.Add(wireCapsule);
        }
    }
}
