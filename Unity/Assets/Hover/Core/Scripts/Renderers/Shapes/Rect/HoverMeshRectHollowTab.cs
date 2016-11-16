using Hover.Core.Utils;
using UnityEngine;

namespace Hover.Core.Renderers.Shapes.Rect {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverIndicator))]
	[RequireComponent(typeof(HoverShapeRect))]
	public class HoverMeshRectHollowTab : HoverMeshRect {

		public SizeType InnerSizeType = SizeType.Min;
		public float TabOutward = 0.01f;
		public float TabThickness = 0.025f;
		public bool ShowTabN = true;
		public bool ShowTabE = false;
		public bool ShowTabS = false;
		public bool ShowTabW = false;

		private SizeType vPrevInnerType;
		private float vPrevTabOut;
		private float vPrevTabThick;
		private bool vPrevTabN;
		private bool vPrevTabE;
		private bool vPrevTabS;
		private bool vPrevTabW;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override bool IsMeshVisible {
			get {
				HoverShapeRect shape = GetComponent<HoverShapeRect>();
				float innerProg = GetDimensionProgress(InnerSizeType);
				float outerProg = GetDimensionProgress(OuterSizeType);
				return (shape.SizeX != 0 && shape.SizeY != 0 && outerProg != innerProg);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override bool ShouldUpdateMesh() {
			bool shouldUpdate = (
				base.ShouldUpdateMesh() ||
				InnerSizeType != vPrevInnerType ||
				TabOutward != vPrevTabOut ||
				TabThickness != vPrevTabThick ||
				vPrevTabN != ShowTabN ||
				vPrevTabE != ShowTabE ||
				vPrevTabS != ShowTabS ||
				vPrevTabW != ShowTabW
			);

			vPrevInnerType = InnerSizeType;
			vPrevTabOut = TabOutward;
			vPrevTabThick = TabThickness;
			vPrevTabN = ShowTabN;
			vPrevTabE = ShowTabE;
			vPrevTabS = ShowTabS;
			vPrevTabW = ShowTabW;

			return shouldUpdate;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void UpdateMesh() {
			HoverShapeRect shape = GetComponent<HoverShapeRect>();
			float innerProg = GetDimensionProgress(InnerSizeType);
			float outerProg = GetDimensionProgress(OuterSizeType);
			float outerW;
			float outerH;
			float innerW;
			float innerH;

			if ( shape.SizeX >= shape.SizeY ) {
				outerH = shape.SizeY*outerProg;
				innerH = shape.SizeY*innerProg;
				outerW = shape.SizeX-(shape.SizeY-outerH);
				innerW = shape.SizeX-(shape.SizeY-innerH);
			}
			else {
				outerW = shape.SizeX*outerProg;
				innerW = shape.SizeX*innerProg;
				outerH = shape.SizeY-(shape.SizeX-outerW);
				innerH = shape.SizeY-(shape.SizeX-innerW);
			}

			MeshUtil.BuildHollowRectangleTabMesh(vMeshBuild, outerW, outerH, innerW, innerH,
				TabOutward*outerProg, TabThickness*outerProg, ShowTabN, ShowTabE, ShowTabS, ShowTabW);

			UpdateAutoUv(shape, outerW, outerH);
			UpdateMeshUvAndColors();
			vMeshBuild.Commit();
			vMeshBuild.CommitColors();
		}

	}

}
