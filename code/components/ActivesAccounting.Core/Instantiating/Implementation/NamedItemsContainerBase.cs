// === CETITEC GmbH =================================================================
// 
//  File:      NamedItemsContainerBase.cs
// 
//             COPYRIGHT (c) 2012-2022 by CETITEC GmbH
//             All rights reserved.
// 
// ==============================================================================
// Copyright 2022 CETITEC GmbH ('CETITEC'). All rights reserved.
// Please make sure that all information within a document marked as 'Confidential'
// or 'Restricted Access' is handled solely in accordance with the agreement
// pursuant to which it is provided, and is not reproduced or disclosed to others
// without the prior written consent of CETITEC. The confidential ranking of a document
// can be found in the footer of every page. This document supersedes and replaces
// all information previously supplied. The technical information in this document
// loses its validity with the next edition. Although the information is believed
// to be accurate, no responsibility is assumed for inaccuracies. Specifications
// and other documents mentioned in this document are subject to change without
// notice. CETITEC reserves the right to make changes to this document and to the
// products at any time without notice. Neither the provision of this information
// nor the sale of the described products conveys any licenses under any patent
// rights or other intellectual property rights of CETITEC or others. The products
// may contain design defects or errors known as anomalies, including but not
// necessarily limited to any which may be identified in this document, which may
// cause the product to deviate from published descriptions. Anomalies are
// described in errata sheets available upon request. CETITEC products are not
// designed, intended, authorized or warranted for use in any life support or
// other application where product failure could cause or contribute to personal
// injury or severe property damage. Any and all such uses without prior written
// approval of an officer of CETITEC will be fully at your own risk. The CETITEC logo is
// a trademark of CETITEC Other names mentioned may be trademarks of their respective
// holders.
// 
// CETITEC disclaims and excludes any and all warranties, including without limitation
// any and all implied warranties of merchantability, fitness for a particular
// purpose, title, and against infringement and the like, and any and all
// warranties arising from any course of dealing or usage of trade. In no event
// shall CETITEC be liable for any direct, incidental, indirect, special, punitive,
// or consequential damages; or for lost data, profits, savings or revenues of
// any kind; regardless of the form of action, whether based on contract; tort;
// negligence of CETITEC or others; strict liability; breach of warranty; or
// otherwise; whether or not any remedy of buyer is held to have failed of its
// essential purpose, and whether or not CETITEC has been advised of the possibility
// of such damages.
// ==============================================================================

using System;
using System.Linq;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal abstract class NamedItemsContainerBase<T> : ContainerBase<T> where T : IUniqueItem, INamedItem
{
    protected void ValidateUniqueName(string aName)
    {
        if (Items.Any(aItem => aItem.Name.Equals(aName, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw Exceptions.AlreadyHasItem(ItemName, aName, "Name");
        }
    }
}