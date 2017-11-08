using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FloatingToolbar : MonoBehaviour
{
    // Container
    public GameObject Container;

    // Buttons
    public Button ButtonExplore;
    public Button ButtonBuildKeep;
    public Button ButtonBuildFarm;
    public Button ButtonDestroy;

    // Private state
    private CellController selectedCell;


    // START
    // ========================================================================

    /// <summary>
    /// Start routine.
    /// </summary>
    public void Start()
    {
        // Assertions
        Debug.Assert(ButtonExplore != null, "ButtonExplore was not found.");
        Debug.Assert(ButtonBuildKeep != null, "ButtonBuildKeep was not found.");
        Debug.Assert(ButtonBuildFarm != null, "ButtonBuildFarm was not found.");
        Debug.Assert(ButtonDestroy != null, "ButtonDestroy was not found.");

        // Events
        ButtonExplore.onClick.AddListener(OnButtonExploreClicked);
        ButtonBuildKeep.onClick.AddListener(OnButtonBuildKeepClicked);
        ButtonBuildFarm.onClick.AddListener(OnButtonBuildFarmClicked);
        ButtonDestroy.onClick.AddListener(OnButtonDestroyClicked);

        // Notifications
        NotificationManager.Subscribe(CellNotifications.Selected, OnCellSelected);
        NotificationManager.Subscribe(CellNotifications.Deselected, OnCellDeselected);

        // Initial state
        Hide();
    }


    // PUBLIC
    // ========================================================================

    /// <summary>
    /// Shows the floating toolbar.
    /// </summary>
    public void Show()
    {
        // Show
        gameObject.SetActive(true);

        // Explore state
        ButtonExplore.gameObject.SetActive(selectedCell.Grid.HasUnexploredNeighbors(selectedCell));

        // Build states
        SetBuildButtonsActive(selectedCell.Building == null);
    }

    /// <summary>
    /// Hides the floating toolbar.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the active state of any build buttons.
    /// </summary>
    public void SetBuildButtonsActive(bool value)
    {
        ButtonBuildKeep.gameObject.SetActive(value);
        ButtonBuildFarm.gameObject.SetActive(value);
    }

    /// <summary>
    /// Update routine.
    /// </summary>
    public void Update()
    {
        if (selectedCell)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(selectedCell.transform.position);

            Container.transform.position = screenPosition;
            Container.transform.Translate(new Vector3(0, -50f));
        }
    }


    // EVENTS
    // ========================================================================

    /// <summary>
    /// Button: Explore
    /// </summary>
    private void OnButtonExploreClicked()
    {
        ButtonExplore.gameObject.SetActive(false);
        NotificationManager.Broadcast(ToolbarNotifications.Explore, selectedCell);
    }

    /// <summary>
    /// Button: Build keep
    /// </summary>
    private void OnButtonBuildKeepClicked()
    {
        SetBuildButtonsActive(false);
        NotificationManager.Broadcast(ToolbarNotifications.BuildKeep);
    }

    /// <summary>
    /// Button: Build farm
    /// </summary>
    private void OnButtonBuildFarmClicked()
    {
        SetBuildButtonsActive(false);
        NotificationManager.Broadcast(ToolbarNotifications.BuildFarm);
    }

    /// <summary>
    /// Button: Destroy
    /// </summary>
    private void OnButtonDestroyClicked()
    {

    }


    // NOTIFICATIONS
    // ========================================================================

    /// <summary>
    /// Cell selected.
    /// </summary>
    /// <param name="data"></param>
    private void OnCellSelected(object data)
    {
        selectedCell = (CellController)data;
        Show();
    }

    /// <summary>
    /// Cell deselected.
    /// </summary>
    /// <param name="data"></param>
    private void OnCellDeselected(object data)
    {
        selectedCell = null;
        Hide();
    }
}