using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Hangar
{
	public class HangarColorPaletButton : MonoBehaviour
	{
		[SerializeField] public int id = 0;

		private HangarCtrl hangarCtrl;


		void Start()
		{
			hangarCtrl = GameObject.Find("HangarCtrl").GetComponent<HangarCtrl>();  // <- prefabで個別設定しない為
		}

		void Update()
		{

		}

		public void OnClick()
		{
			if (HCommon.hangarSelectTarget != HangarSelectTarget.MENU_SELECT_COLOR_PALET) { return; }
			Debug.Log("## ColorPaletButton押された ID:" + id);

			HangarColorPaletMode idMode = HangarColorPaletMode.PALET;
			switch (id)
			{
				case 0: idMode = HangarColorPaletMode.PALET; break;
				case 1: idMode = HangarColorPaletMode.CODE; break;
				case 2: idMode = HangarColorPaletMode.HISTORY; break;
				case 3: idMode = HangarColorPaletMode.FAVARITE; break;
				case 4: idMode = HangarColorPaletMode.CONFIRM; break;
			}

			if (idMode != HCommon.selectColorPaletMode)
			{
				HCommon.selectColorPaletMode = idMode;
				hangarCtrl.ColorPaletCtrl.setCursol();
			}
			else
			{
				// Padの場合は、ここで捕捉
				if (idMode == HangarColorPaletMode.HISTORY)
				{
					HCommon.RGBtolocalHSV(
							HCommon.historyPalet[HCommon.selectColorPaletHistorySelect].r,
							HCommon.historyPalet[HCommon.selectColorPaletHistorySelect].g,
							HCommon.historyPalet[HCommon.selectColorPaletHistorySelect].b,
							out HCommon.selectColorPaletH,
							out HCommon.selectColorPaletS,
							out HCommon.selectColorPaletV
					);
					hangarCtrl.ColorPaletCtrl.setCursol();
				}
				if (idMode == HangarColorPaletMode.FAVARITE)
				{
					HCommon.RGBtolocalHSV(
							HCommon.favoritePalet[HCommon.selectColorPaletFavoriteSelect].r,
							HCommon.favoritePalet[HCommon.selectColorPaletFavoriteSelect].g,
							HCommon.favoritePalet[HCommon.selectColorPaletFavoriteSelect].b,
							out HCommon.selectColorPaletH,
							out HCommon.selectColorPaletS,
							out HCommon.selectColorPaletV
					);
					hangarCtrl.ColorPaletCtrl.setCursol();
				}
			}

			// 確定ボタン
			if (id == 4)
			{
				// ここで色等を確定してパレットトップへ
				int wno = 0;
				switch (HCommon.hangarColorSelectTopSelect)
				{
					case HangarColorSelectTop.COLOR1: wno = 0; break;
					case HangarColorSelectTop.COLOR2: wno = 1; break;
					case HangarColorSelectTop.COLOR3: wno = 2; break;
					case HangarColorSelectTop.COLOR4: wno = 3; break;
					case HangarColorSelectTop.COLOR5: wno = 4; break;
					case HangarColorSelectTop.COLOR6: wno = 5; break;
					case HangarColorSelectTop.COLOR7: wno = 6; break;
					case HangarColorSelectTop.COLOR8: wno = 7; break;
				}
				FramePartsItem fp = HCommon.getPartsItem();
				HCommon.localHSVtoRGB(
					HCommon.selectColorPaletH,
					HCommon.selectColorPaletS,
					HCommon.selectColorPaletV,
					out fp.palet[wno].r,
					out fp.palet[wno].g,
					out fp.palet[wno].b
					);
				HCommon.setPartsItem(fp);


				// ヒストリーへ登録
				bool flg = false;
				float r, g, b;
				HCommon.localHSVtoRGB(
						HCommon.selectColorPaletH,
						HCommon.selectColorPaletS,
						HCommon.selectColorPaletV,
						out r,
						out g,
						out b
					);
				for (var i = 0; i < HCommon.historyPalet.Count; i++)
				{
					if ((HCommon.historyPalet[i].r == r)
						&& (HCommon.historyPalet[i].g == g)
						&& (HCommon.historyPalet[i].b == b))
					{
						flg = true;
						break;
					}
				}
				if (!flg)
				{
					for (var i = HCommon.historyPalet.Count - 1; i > 0; i--)
					{
						ColorPalet tmp;
						tmp = HCommon.historyPalet[i];
						HCommon.historyPalet[i] = HCommon.historyPalet[i-1];
						HCommon.historyPalet[i-1] = tmp;
					}
					HCommon.historyPalet[0].r = r;
					HCommon.historyPalet[0].g = g;
					HCommon.historyPalet[0].b = b;
				}
				hangarCtrl.ReturnColorTop();
			}

		}

	}
}