using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FOSgamemanager : MonoBehaviour {

    [Header("UI Elements")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image evelSelectPrefab;

    [Header("Jigsaw Settings")]
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform piecePrefab; // Added missing variable!
    [SerializeField] private Transform gameHolder;   // Added missing variable!

    private List<Transform> pieces;
    private Vector2Int dimensions;

    void Start() {
        // Create the UI Selection Menu
        foreach (Texture2D texture in imageTextures) {
            Image newImage = Instantiate(evelSelectPrefab, levelSelectPanel);
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            newImage.sprite = newSprite;

            // Set up button selection to trigger the game load
            newImage.GetComponent<Button>().onClick.AddListener(delegate { StartGame(texture); });
        }
    }

    public void StartGame(Texture2D jigsawTexture) {
        // Hide UI selection panel
        levelSelectPanel.gameObject.SetActive(false);

        pieces = new List<Transform>();
        dimensions = GetDimensions(jigsawTexture, difficulty);

        // Generate the puzzle board pieces
        CreateJigsawPieces(jigsawTexture);
    }

    Vector2Int GetDimensions(Texture2D texture, int difficulty) {
        Vector2Int dimensions = Vector2Int.zero;

        if (texture.width < texture.height) {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * texture.height) / texture.width;
        } else {
            dimensions.x = (difficulty * texture.width) / texture.height;
            dimensions.y = difficulty;
        }

        return dimensions;
    }

    void CreateJigsawPieces(Texture2D jigsawTexture) {
        // Calculate the size of each piece relative to total texture scale
        float height = 1f / dimensions.y;
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        float width = aspect / dimensions.x;

      for (int row = 0; row < dimensions.y; row++) {
            for (int col = 0; col < dimensions.x; col++) {
                // Create the piece game object container inside the holder scene graph
                Transform piece = Instantiate(piecePrefab, gameHolder);

                // Fixed typo: changed .locationPosition to .localPosition
                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2f) + (width * col) + (width / 2f),
                    (-height * dimensions.y / 2f) + (height * row) + (height / 2f),
                    -1f
                );

                piece.localScale = new Vector3(width, height, 1f);

                // Give it an identifier index name
                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);
            }
        }
    }
}

