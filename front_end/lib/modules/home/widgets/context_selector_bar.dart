import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../view_models/farm_context_view_model.dart';
import 'context_selector_drawer.dart';

class ContextSelectorBar extends StatelessWidget {
  final String? barnEmoji;
  final String? barnLabel;
  final String? batchLabel;
  final String? groupTag;
  final String? groupLabel;
  final VoidCallback? onChange;

  const ContextSelectorBar({
    super.key,
    this.barnEmoji,
    this.barnLabel,
    this.batchLabel,
    this.groupTag,
    this.groupLabel,
    this.onChange,
  });

  @override
  Widget build(BuildContext context) {
    final farmContext = context.watch<FarmContextViewModel>();

    final currentBarnEmoji = barnEmoji ?? (farmContext.selectedGalpao['emoji'] as String);
    final currentBarnLabel = barnLabel ?? (farmContext.selectedGalpao['label'] as String);
    final currentBatchLabel = batchLabel ?? (farmContext.selectedLote['label'] as String);
    final currentGroupTag = groupTag ?? (farmContext.selectedAnimal['tag'] as String);
    final currentGroupLabel = groupLabel ?? (farmContext.selectedAnimal['label'] as String);
    final currentOnChange = onChange ?? () => ContextSelectorDrawer.show(context);

    return InkWell(
      onTap: currentOnChange,
      child: Container(
        width: double.infinity,
        padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
        decoration: const BoxDecoration(
          color: Colors.white,
          border: Border(
            bottom: BorderSide(
              color: Color(0xFFEEEEEE),
              width: 1,
            ),
          ),
        ),
        child: Row(
          children: [
            // Galpão pill
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
              decoration: BoxDecoration(
                color: const Color(0xFFFFF8E1), // Light amber
                borderRadius: BorderRadius.circular(12),
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Text(
                    currentBarnEmoji,
                    style: const TextStyle(fontSize: 14),
                  ),
                  const SizedBox(width: 4),
                  Text(
                    currentBarnLabel,
                    style: const TextStyle(
                      fontSize: 11,
                      fontWeight: FontWeight.w900,
                      color: Color(0xFF8D6E63), // Brownish amber
                    ),
                  ),
                ],
              ),
            ),
            const SizedBox(width: 6),
            const Icon(Icons.chevron_right, size: 14, color: Colors.grey),
            const SizedBox(width: 6),

            // Lote pill
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
              decoration: BoxDecoration(
                color: const Color(0xFFF5F5F5),
                borderRadius: BorderRadius.circular(12),
              ),
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Icon(Icons.inventory_2_outlined, size: 12, color: Colors.grey),
                  const SizedBox(width: 4),
                  Flexible(
                    child: Text(
                      currentBatchLabel,
                      overflow: TextOverflow.ellipsis,
                      style: const TextStyle(
                        fontSize: 11,
                        fontWeight: FontWeight.bold,
                        color: Color(0xFF616161),
                      ),
                    ),
                  ),
                ],
              ),
            ),
            const SizedBox(width: 6),
            const Icon(Icons.chevron_right, size: 14, color: Colors.grey),
            const SizedBox(width: 6),

            // Grupo/Animal info
            Expanded(
              child: Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Text(
                    currentGroupTag,
                    style: const TextStyle(
                      fontSize: 13,
                      fontWeight: FontWeight.bold,
                      color: Color(0xFF1A1A1A),
                    ),
                  ),
                  const SizedBox(width: 4),
                  Flexible(
                    child: Text(
                      currentGroupLabel,
                      overflow: TextOverflow.ellipsis,
                      style: const TextStyle(
                        fontSize: 11,
                        fontWeight: FontWeight.w500,
                        color: Color(0xFF757575),
                      ),
                    ),
                  ),
                ],
              ),
            ),

            // Alterar button
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
              decoration: BoxDecoration(
                color: const Color(0xFFE8F5E9),
                borderRadius: BorderRadius.circular(12),
                border: Border.all(color: const Color(0xFFC8E6C9)),
              ),
              child: const Row(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Icon(Icons.edit, size: 12, color: Color(0xFF2E7D32)),
                  SizedBox(width: 4),
                  Text(
                    'Alterar',
                    style: TextStyle(
                      fontSize: 11,
                      fontWeight: FontWeight.bold,
                      color: Color(0xFF1B5E20),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
